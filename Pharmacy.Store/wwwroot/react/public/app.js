if ('serviceWorker' in navigator) {
    window.addEventListener('load', function() {
      var currentUrl = window.location;
      navigator.serviceWorker.register(currentUrl.origin+'/worker.js').then(function(registration) {
        console.log('Worker registration successful', registration.scope);
      }, function(err) {
        console.log('Worker registration failed', err);
      }).catch(function(err) {
        console.log(err);
      });
    });
  } else {
    console.log('Service Worker is not supported by browser.');
  }