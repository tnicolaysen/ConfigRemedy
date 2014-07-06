'use strict';

describe('Controller: EnvironmentsCtrl', function () {

  // load the controller's module
  beforeEach(module('configuratronclientApp'));

  var EnvironmentsCtrl,
    scope;

  // Initialize the controller and a mock scope
  beforeEach(inject(function ($controller, $rootScope) {
    scope = $rootScope.$new();
    EnvironmentsCtrl = $controller('EnvironmentsCtrl', {
      $scope: scope
    });
  }));

  it('should attach a list of awesomeThings to the scope', function () {
    expect(scope.awesomeThings.length).toBe(3);
  });
});
