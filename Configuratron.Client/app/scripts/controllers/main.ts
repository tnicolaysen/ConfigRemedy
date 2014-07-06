'use strict';

/**
 * @ngdoc function
 * @name ctronApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the ctronApp
 */
angular.module('ctronApp')
  .controller('MainCtrl', ($scope) => {
    $scope.awesomeThings = [
      'HTML5 Boilerplate',
      'AngularJS',
      'Karma'
    ];
  });
