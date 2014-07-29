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
    .controller('TopLevelController', function ($scope, $log, $window, UserRoles, AuthService, DiagnosticsService) {
        $scope.currentUser = null;
        $scope.userRoles = UserRoles;
        $scope.isAuthorized = AuthService.isAuthorized;
        $scope.systemInfo = DiagnosticsService.getDiagnostics();

        $scope.setCurrentUser = function (user) {
            $scope.currentUser = user;
            $window.sessionStorage.token = user.token;
        };
    });