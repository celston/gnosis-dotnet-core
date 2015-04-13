define(['./app'], function (app) {
    app.config(['$routeProvider', function ($routeProvider) {
        $routeProvider
            .when('/index', {
                templateUrl: 'index.html',
                controller: 'IndexController'
            })
            .when('/banana/create', {
                templateUrl: 'fruits/template/bananaCreate',
                controller: 'BananaCreateController'
            })
            .otherwise({
                redirectTo: '/index'
            });
    }]);
})