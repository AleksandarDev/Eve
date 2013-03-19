var Eve;
(function (Eve) {
    (function (API) {
        (function (Auth) {
            var OpenID = (function () {
                function OpenID() { }
                return OpenID;
            })();            
        })(API.Auth || (API.Auth = {}));
        var Auth = API.Auth;
    })(Eve.API || (Eve.API = {}));
    var API = Eve.API;
})(Eve || (Eve = {}));
