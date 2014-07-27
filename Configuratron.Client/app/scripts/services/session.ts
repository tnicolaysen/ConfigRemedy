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
        this.create = function (sessionId, userId, userName, userRole) {
            this.id = sessionId;
            this.userId = userId;
            this.userName = userName;
            this.userRole = userRole;
        };
        this.destroy = function () {
            this.id = null;
            this.userId = null;
            this.userName = null;
            this.userRole = null;
        };
        return this;
    });

