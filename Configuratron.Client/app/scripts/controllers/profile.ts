/**
 * Created by evgenyb on 13.08.2014.
 */
/// <reference path='../../typings/_all.d.ts' />
'use strict';

/**
 * @ngdoc function
 * @name ctronApp.controller:ProfileController
 * @description
 * # ProfileController
 * Controller of the ctronApp
 */

angular.module('ctronApp')
    .controller('ProfileController', ($scope, $rootScope, $resource, $http, $window, $log, $modal, configuration, Session) => {
        $scope.environments = [];

        var Users = $resource(configuration.ApiBaseUrl + 'users/:id.json', null, {
            'update': { method: 'PUT' }
        });
        $scope.user = Users.get({id: Session.userName});
        $scope.showPasswordChange = false;
        $scope.password = null;
        $scope.confirmNewPassword = null;

        $scope.save = (user) => {
            $log.log('Saving profile for user ' + user.username);
            $log.log($scope.password);
            var profile = {
                'userName': user.username,
                'displayName': user.displayName,
                'email': user.email,
                'password': $scope.password
            };
            Users.update({id: user.username}, profile, (userIdentity) => {
                $log.log(userIdentity);
                $scope.setCurrentUser(userIdentity);
            });
        };
    });
