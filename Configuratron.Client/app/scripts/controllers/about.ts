'use strict';

/**
 * @ngdoc function
 * @name ctronApp.controller:AboutCtrl
 * @description
 * # AboutCtrl
 * Controller of the ctronApp
 */
angular.module('ctronApp')
  .controller('AboutCtrl', ($scope) => {
    $scope.awesomeThings = [
      'Stuff 2',
      'AngularJS',
      'Karma'
    ];
  });
