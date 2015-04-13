require.config({
    urlArgs: 'cache=' + (new Date()).getTime(),
    paths: {
        'angular': '/Content/vendor/angular/angular.min',
        'angular-route': '/Content/vendor/angular-route/angular-route.min'
    },
    shim: {
        'angular': {
            exports: 'angular'
        },
        'angular-route': {
            deps: ['angular']
        }
    },
    packages: [
        {
            name: 'gnosis-ng-ui',
            location: '../../modules/gnosis-ng-ui'
        }
    ]
})