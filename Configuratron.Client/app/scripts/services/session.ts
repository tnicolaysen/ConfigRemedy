/// <reference path='../../typings/_all.d.ts' />
/**
 * Created by evgenyb on 27.07.2014.
 */
'use strict';

/**
 * @ngdoc function
 * @name ctronApp.service:Session
 * @description
 * # Session
 * Service of the ctronApp
 */


angular.module('ctronApp')
    .service('Session', function () {
        this.create = function (user) {
            this.id = user.token;
            this.userId = user.userId;
            this.userName = user.userName;
            this.displayName = user.displayName;
            this.userRole = user.role;
        };
        this.destroy = function () {
            this.id = null;
            this.userId = null;
            this.displayName = null;
            this.userName = null;
            this.userRole = null;
        };
        return this;
    });

