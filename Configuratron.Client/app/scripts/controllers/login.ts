/// <reference path='../../typings/_all.d.ts' />
/**
 * Created by evgenyb on 27.07.2014.
 */

/**
 * @ngdoc function
 * @name ctronApp.controller:LoginCtrl
 * @description
 * # LoginCtrl
 * Controller of the ctronApp
 */

angular.module('ctronApp')
    .controller('LoginController', function ($scope, $rootScope, $log, AuthenticationEvents, AuthService) {
        $scope.credentials = {
            username: '',
            password: ''
        };
        $scope.login = function (credentials) {
            AuthService.login(credentials).then(function (user) {
                $rootScope.$broadcast(AuthenticationEvents.loginSuccess);
                $scope.setCurrentUser(user);
            }, function () {
                $rootScope.$broadcast(AuthenticationEvents.loginFailed);
            });
        };
    });

