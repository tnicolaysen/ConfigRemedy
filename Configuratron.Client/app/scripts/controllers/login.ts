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
    .controller('LoginController', function ($scope, $rootScope, $log, $state, AuthenticationEvents, AuthService) {
        $scope.credentials = {
            username: '',
            password: '',
            rememberMe: true
        };
        $scope.login = function (credentials) {
            AuthService.login(credentials).then(function (user) {
                $scope.setCurrentUser(user, credentials.rememberMe);
                $state.go('main');
                $rootScope.$broadcast(AuthenticationEvents.loginSuccess);
            }, function () {
                $rootScope.$broadcast(AuthenticationEvents.loginFailed);
            });
        };
    });

