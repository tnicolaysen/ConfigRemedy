/// <reference path='../../typings/_all.d.ts' />

'use strict';

/**
 * @ngdoc interceptor
 * @name ctronApp.authInterceptor
 * @description
 * # authInterceptor
 * Service in the ctronApp.
 */
angular.module('ctronApp')
    .factory('authInterceptor', function ($rootScope, $q, $window, $log) {
        return {
            request: function (config) {
                config.headers = config.headers || {};
                if ($window.sessionStorage.token) {
                    config.headers.Authorization = 'Token ' + $window.sessionStorage.token;
                }
                return config;
            },
            response: function (response) {
                if (response.status === 401) {
                    // handle the case where the user is not authenticated
                }
                return response || $q.when(response);
            }
        };
    });