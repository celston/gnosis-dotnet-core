define(['../app', 'baseUrl'], function (app, baseUrl) {
    function DataService($http) {
        this.$http = $http;
    }

    DataService.prototype.addBanana = function (data) {
        return this.$http.post(baseUrl + 'Fruits/Home/CreateBanana', {
            data: data
        });
    }

    DataService.prototype.foo = function () {
        return 'The quick brown fox ran over the lazy dog.';
    }

    app.service('dataService', DataService);
})