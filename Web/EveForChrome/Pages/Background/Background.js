var RunAt = (function () {
    function RunAt() { }
    RunAt.Start = "document_start";
    RunAt.End = "document_end";
    RunAt.Idle = "document_idle";
    return RunAt;
})();
var Background = (function () {
    function Background() {
        this.webSocketPort = "41258";
        this.webSocketHost = "ws://localhost";
        this.InitializeWebSockets();
    }
    Background.prototype.InitializeWebSockets = function () {
        this.webSocketHost += ":" + this.webSocketPort;
        this.webSocket = new WebSocket(this.webSocketHost);
        this.webSocket.onmessage = this.WebSocketOnMessage.bind(this);
        this.webSocket.onopen = this.WebSocketOnOpen.bind(this);
        this.webSocket.onclose = this.WebSocketOnClose.bind(this);
        this.webSocket.onerror = this.WebSocketOnError.bind(this);
    };
    Background.prototype.WebSocketOnMessage = function (message) {
        var _this = this;
        try  {
            console.log("Got message: ");
            console.log(message);
            if(message.data == "grooveshark:next-song") {
                chrome.tabs.query({
                    url: "http://*.grooveshark.com/*"
                }, function (tab) {
                    _this.ExecuteScript(tab[0].id, '$("#play-next").click();');
                });
            } else if(message.data == "grooveshark:prev-song") {
                chrome.tabs.query({
                    url: "http://*.grooveshark.com/*"
                }, function (tab) {
                    _this.ExecuteScript(tab[0].id, '$("#play-prev").click();');
                });
            } else if(message.data == "grooveshark:pause-song") {
                chrome.tabs.query({
                    url: "http://*.grooveshark.com/*"
                }, function (tab) {
                    _this.ExecuteScript(tab[0].id, 'if($("#play-pause").hasClass("playing")) $("#play-pause").click();');
                });
            } else if(message.data == "grooveshark:resume-song") {
                chrome.tabs.query({
                    url: "http://*.grooveshark.com/*"
                }, function (tab) {
                    _this.ExecuteScript(tab[0].id, 'if(!$("#play-pause").hasClass("playing")) $("#play-pause").click();');
                });
            }
        } catch (exception) {
            console.warn(exception);
        }
    };
    Background.prototype.WebSocketOnOpen = function (event) {
        console.log("Socket opened to [" + this.webSocketHost + "]");
    };
    Background.prototype.WebSocketOnClose = function (event) {
        console.log("Socket closed!");
    };
    Background.prototype.WebSocketOnError = function (evet) {
        console.error(event);
    };
    Background.prototype.ExecuteScript = function (tabID, code, deepInject, runAt) {
        if (typeof deepInject === "undefined") { deepInject = false; }
        if (typeof runAt === "undefined") { runAt = RunAt.End; }
        chrome.tabs.executeScript(tabID, {
            code: "var scriptToInject = document.createElement('script'); scriptToInject.type = 'text/javascript'; scriptToInject.innerHTML = '" + code + "'; document.getElementsByTagName('head')[0].appendChild(scriptToInject);",
            allFrames: deepInject,
            runAt: "document_end"
        }, this.ScriptExecutionCallback);
    };
    Background.prototype.ScriptExecutionCallback = function (result) {
        console.log("Script executed!");
        if(result !== undefined) {
            console.log("Script results: ");
            console.log(result);
        }
    };
    return Background;
})();
var background = new Background();
