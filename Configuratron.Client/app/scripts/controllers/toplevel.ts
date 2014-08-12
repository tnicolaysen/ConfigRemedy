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
    .controller('TopLevelController', function ($scope, $log, $window, UserRoles, AuthService, DiagnosticsService, DSCacheFactory, Session) {
        $scope.currentUser = null;
        $scope.userRoles = UserRoles;
        $scope.isAuthorized = AuthService.isAuthorized;
        $scope.systemInfo = DiagnosticsService.getDiagnostics();


        var cache = DSCacheFactory('Configuratron.Cache', {
            deleteOnExpire: 'aggressive',
            storageMode: 'localStorage',
            maxAge: 1209600000 /* 14 days -> move to constants*/
        });

        var user = cache.get('user');
        if (user)
        {
            Session.create(user);
            $scope.currentUser = user;
            $window.sessionStorage.token = user.token;
        }

        $scope.setCurrentUser = function (user) {
            $scope.currentUser = user;
            $window.sessionStorage.token = user.token;
            cache.put('user', user);
        };
    });
