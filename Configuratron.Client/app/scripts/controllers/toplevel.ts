/// <reference path='../../typings/_all.d.ts' />
/**
 * Created by evgenyb on 27.07.2014.
 */

/**
 * @ngdoc function
 * @name ctronApp.controller:TopLevelController
 * @description
 * # TopLevelController
 * Controller of the ctronApp
 */

angular.module('ctronApp')
    .controller('TopLevelController', function ($scope, $log, $window, UserRoles, AuthService) {
        $scope.currentUser = null;
        $scope.userRoles = UserRoles;
        $scope.isAuthorized = AuthService.isAuthorized;
        $scope.version = "0.0.12";

        $scope.setCurrentUser = function (user) {
            $scope.currentUser = user;
            $window.sessionStorage.token = user.token;
            $log.log($window.sessionStorage.token);
        };
    });