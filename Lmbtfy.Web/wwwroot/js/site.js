(function () {
    const body = document.body;

    function updateCurrentYear() {
        const year = new Date().getFullYear().toString();
        document.querySelectorAll("[data-current-year]").forEach(function (element) {
            element.textContent = year;
        });
    }

    function setBackground(imageUrl) {
        ["bgDiv", "bgDivFull"].forEach(function (id) {
            const target = document.getElementById(id);
            if (target && imageUrl) {
                target.style.backgroundImage = "url(" + imageUrl + ")";
            }
        });
    }

    async function loadBackground() {
        const cacheKey = "lmbtfy-background";
        const todayKey = new Date().toISOString().slice(0, 10);

        try {
            const cached = localStorage.getItem(cacheKey);
            if (cached) {
                const payload = JSON.parse(cached);
                if (payload.date === todayKey && payload.imageUrl) {
                    setBackground(payload.imageUrl);
                    return;
                }
            }
        }
        catch {
        }

        try {
            const response = await fetch("/api/background", { headers: { "Accept": "application/json" } });
            if (!response.ok) {
                return;
            }

            const payload = await response.json();
            if (payload && payload.imageUrl) {
                setBackground(payload.imageUrl);
                localStorage.setItem(cacheKey, JSON.stringify({ date: todayKey, imageUrl: payload.imageUrl }));
            }
        }
        catch {
        }
    }

    function showResult() {
        const result = $("#lmbtfyResult");
        result.addClass("is-visible");
        if ($.fn.show) {
            result.stop(true, true).show("bounce", "fast");
            return;
        }

        result.show();
    }

    function hideResult() {
        const result = $("#lmbtfyResult");
        result.removeClass("is-visible").hide();
    }

    function escapeHtml(value) {
        return value
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/\"/g, "&quot;")
            .replace(/'/g, "&#39;");
    }

    function renderShareResult(result) {
        return "<div class=\"center\">"
            + "<h1>Share the link below!</h1>"
            + "<input id=\"lmbtfyLink\" value=\"" + escapeHtml(result.tinyUrl) + "\" readonly />"
            + "<p>Alternatively, provide the direct link: <strong><a href=\"" + escapeHtml(result.url) + "\" title=\"The Url\">" + escapeHtml(result.url) + "</a></strong><br />"
            + "The URL <a href=\"" + escapeHtml(result.tinyUrl) + "\" title=\"Tiny URL\">" + escapeHtml(result.tinyUrl) + "</a> was shortened using <a href=\"https://tinyurl.com\" title=\"Tiny Url\">TinyUrl.com</a>."
            + "</p>"
            + "</div>";
    }

    function renderError(message) {
        return "<div class=\"result-error center\"><p>" + escapeHtml(message) + "</p></div>";
    }

    function parseFriendlyPath(pathname) {
        const trimmedPath = pathname.replace(/^\/+|\/+$/g, "");
        if (!trimmedPath || trimmedPath.toLowerCase() === "about" || trimmedPath.indexOf(".") >= 0) {
            return "";
        }

        try {
            return decodeURIComponent(trimmedPath)
                .replace(/[+/_-]+/g, " ")
                .replace(/\s+/g, " ")
                .trim();
        }
        catch {
            return trimmedPath
                .replace(/[+/_-]+/g, " ")
                .replace(/\s+/g, " ")
                .trim();
        }
    }

    function getEffectiveQuery() {
        const params = new URLSearchParams(window.location.search);
        const query = params.get("q");
        if (query && query.trim()) {
            return query.trim();
        }

        const friendlyPathQuery = parseFriendlyPath(window.location.pathname);
        if (friendlyPathQuery) {
            window.history.replaceState({}, "", "/?q=" + encodeURIComponent(friendlyPathQuery));
            return friendlyPathQuery;
        }

        return "";
    }

    function activateLandingMode() {
        body.dataset.mode = "landing";
        $("#homeInstructions").show();
        hideResult();

        const form = document.getElementById("sb_form");
        form.action = "/api/generate-url";
        form.method = "post";
    }

    function activateQueryMode(query) {
        body.dataset.mode = "bing";

        const form = document.getElementById("sb_form");
        const result = document.getElementById("lmbtfyResult");
        const template = document.getElementById("bingResultTemplate");

        form.action = "https://www.bing.com/search";
        form.method = "get";
        result.innerHTML = template ? template.innerHTML : "";
        $("#homeInstructions").hide();
        showResult();

        if (window.startLmbtfy) {
            window.startLmbtfy(query);
        }
    }

    async function handleHomeSubmit(event) {
        if (body.dataset.mode === "bing") {
            return;
        }

        event.preventDefault();

        const queryInput = document.getElementById("sb_form_q");
        const query = queryInput.value.trim();
        const result = document.getElementById("lmbtfyResult");
        if (!query) {
            hideResult();
            return;
        }

        try {
            const response = await fetch("/api/generate-url", {
                method: "POST",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    query: query,
                    origin: window.location.origin
                })
            });

            const payload = await response.json();
            if (!response.ok) {
                throw new Error(payload.error || "The link could not be generated.");
            }

            result.innerHTML = renderShareResult(payload);
            showResult();

            const linkInput = document.getElementById("lmbtfyLink");
            if (linkInput) {
                linkInput.addEventListener("focus", function () {
                    linkInput.select();
                });
            }
        }
        catch (error) {
            const message = error instanceof Error ? error.message : "The link could not be generated.";
            result.innerHTML = renderError(message);
            showResult();
        }
    }

    function initializeHomePage() {
        const form = document.getElementById("sb_form");
        if (!form) {
            return;
        }

        form.addEventListener("submit", handleHomeSubmit);

        const effectiveQuery = getEffectiveQuery();
        if (effectiveQuery) {
            activateQueryMode(effectiveQuery);
            return;
        }

        activateLandingMode();
    }

    updateCurrentYear();
    loadBackground();

    if (body.dataset.page === "home") {
        initializeHomePage();
    }
})();