/// <reference path='../../typings/_all.d.ts' />
/**
 * Created by evgenyb on 27.07.2014.
 */
'use strict';

/**
 * @ngdoc function
 * @name ctronApp.service:AuthService
 * @description
 * # AuthService
 * Service of the ctronApp
 */

angular.module('ctronApp')
    .factory('AuthService', ($http,  $log, Session, configuration, UserRoles) => {
        var authService = authService || {};

        authService.login = function (credentials) {
            return $http
                .post(configuration.ApiBaseUrl + 'login', credentials)
                .then(function (res) {
                    var user = res.data;
                    Session.create(user);
                    return user;
                });
        };

        authService.isAuthenticated = function () {
            return !!Session.userId;
        };

        authService.isAuthorized = function (authorizedRoles) {
            if (!angular.isArray(authorizedRoles)) {
                authorizedRoles = [authorizedRoles];
            }
            return (authService.isAuthenticated() &&
                authorizedRoles.indexOf(Session.userRole) !== -1);
        };

        authService.isGuest = function (authorizedRoles) {
            if (!angular.isArray(authorizedRoles)) {
                authorizedRoles = [authorizedRoles];
            }
            return authorizedRoles.indexOf(UserRoles.guest) !== -1;
        };

        return authService;
    })
    .constant('AuthenticationEvents', {
        loginSuccess: 'auth-login-success',
        loginFailed: 'auth-login-failed',
        logoutSuccess: 'auth-logout-success',
        sessionTimeout: 'auth-session-timeout',
        notAuthenticated: 'auth-not-authenticated',
        notAuthorized: 'auth-not-authorized'
    })

    .constant('UserRoles', {
        all: '*',
        admin: 'admin',
        guest: 'guest'
    })
;

