define(['./app'], function (app) {
    app.config(['$routeProvider', function ($routeProvider) {
        $routeProvider
            .when('/list', {
                templateUrl: 'widgets/templates/list',
                controller: 'WidgetListController'
            })
            .when('/create', {
                templateUrl: 'widgets/templates/create',
                controller: 'WidgetCreateController'
            })
            .otherwise({
                redirectTo: '/list'
            });
    }]);
})