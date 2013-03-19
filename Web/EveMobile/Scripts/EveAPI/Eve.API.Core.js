var Eve;
(function (Eve) {
    (function (API) {
        (function (Core) {
            var Communication = (function () {
                function Communication() { }
                Communication.prototype.ConnectToWebSocket = function (host, auth) {
                };
                return Communication;
            })();            
        })(API.Core || (API.Core = {}));
        var Core = API.Core;
    })(Eve.API || (Eve.API = {}));
    var API = Eve.API;
})(Eve || (Eve = {}));
