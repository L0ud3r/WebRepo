/*document.addEventListener('click', function(event) {
  var routerOutlet = document.querySelector('router-outlet');
  if (routerOutlet) {
    var dropdowns = routerOutlet.querySelectorAll('.show');
    for (var i = 0; i < dropdowns.length; i++) {
      var openDropdown = dropdowns[i];
      if (!openDropdown.contains(event.target)) {
        openDropdown.classList.remove('show');
      }
    }
  }
});*/

var routerOutlet = document.querySelector('router-outlet');

routerOutlet.addEventListener('click', function(event) {
  var dropdowns = document.getElementsByClassName('show');

  for (var i = 0; i < dropdowns.length; i++) {
    var openDropdown = dropdowns[i];
    if (!openDropdown.contains(event.target)) {
      openDropdown.classList.remove('show');
    }
  }
});

document.addEventListener('click', function(event) {
  var dropdowns = document.getElementsByClassName('show');

  for (var i = 0; i < dropdowns.length; i++) {
    var openDropdown = dropdowns[i];
    if (!openDropdown.contains(event.target)) {
      openDropdown.classList.remove('show');
    }
  }
});
