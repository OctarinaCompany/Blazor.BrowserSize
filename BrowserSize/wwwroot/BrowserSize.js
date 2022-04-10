export function setViewResizeHandler(callback, dotNetHelper) {

    if (callback === null || callback.match(/^ *$/) !== null) {

        console.warn("callback is null or whitespace.");

        return false;
    }

    if (dotNetHelper == null) {

        console.warn("dotNetHelper is null.");

        return false;
    }

    let body = document.querySelector("body");

    if (body) {
        new ResizeObserver(e => {

            let sizes = {
                "body": e[0].contentRect,
                "window": getWindowRect(),
                "screen": getScreenRect()
            };

            dotNetHelper.invokeMethod(callback, sizes);
        })
        .observe(body);
    }
};

function getElementRect(element) {

    if (typeof element === 'string') {

        element = document.querySelector(element);
    }

    if (element == null) {

        console.warn("element is null.");

        return false;
    }

    let rect = element.getBoundingClientRect();

    return rect;
};

function getWindowRect() {

    let rect = {
        "x": 0,
        "y": 0,
        "width": window.innerWidth,
        "height": window.innerHeight
    };

    return rect;
};

function getScreenRect() {

    let rect = {
        "x": 0,
        "y": 0,
        "width": window.screen.width,
        "height": window.screen.height
    };

    return rect;
};


