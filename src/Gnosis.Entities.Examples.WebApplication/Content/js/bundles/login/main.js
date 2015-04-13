require(['../common/require'], function () {
    require([
        'angular',
        './app',
        './controller/login'
    ], function (angular) {
        angular.element(document).ready(function () {
            angular.bootstrap(document, ['login']);
        })
    })
})