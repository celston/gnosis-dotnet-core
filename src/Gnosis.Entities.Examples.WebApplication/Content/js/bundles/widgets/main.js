require(['../common/require'], function () {
    require([
        'angular',
        './app',
        './controller/list',
        './controller/create',
        './config'
    ], function (angular) {
        angular.element(document).ready(function () {
            angular.bootstrap(document, ['widgets']);
        })
    })
})