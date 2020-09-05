var CACHE_NAME = 'pharma-pwa';
var urlsToCache = [
  './',
  './logo.png',
  "./manifest.json",
  "https://unpkg.com/leaflet@1.6.0/dist/leaflet.css",
  "https://cdnjs.cloudflare.com/ajax/libs/slick-carousel/1.6.0/slick.min.css",
  "https://cdnjs.cloudflare.com/ajax/libs/slick-carousel/1.6.0/slick-theme.min.css",
];
// const baseUrl =  'https://pharma.hillavas.com';
// const apiUrl = 'https://pharma.hillavas.com/api/';

const baseUrl = 'http://localhost:3000';
const apiUrl = 'https://localhost:44328/';

// Install a service worker
self.addEventListener('install', e => {
  // Perform install steps
  e.waitUntil(
    caches.open(CACHE_NAME)
      .then(function (cache) {
        console.log('Opened cache');
        return cache.addAll(urlsToCache);
      })
  );
});

// Cache and return requests
self.addEventListener('fetch', e => {

  // console.log('[ServiceWorker] Fetch', e.request.url);
  // if (event.request.mode === 'navigate') {
  //   e.respondWith(caches.match('/index.html'));
  // }
  if (e.request.url.indexOf(apiUrl) === 0)//for api 
  {
    e.respondWith(
      fetch(e.request)
        .then(function (response) {
          return caches.open(CACHE_NAME).then(function (cache) {
            cache.put(e.request.url, response.clone());
            console.log('[ServiceWorker] Fetched&Cached Data');
            return response;
          });
        })
        .catch(function (err) { })
    );
  } else if (e.request.url.indexOf(baseUrl) === 0 || urlsToCache.indexOf(e.request.url) >= 0) {
    //for shell
    e.respondWith(
      caches.open(CACHE_NAME).then(function (cache) {
        return caches.match(e.request.url).then(function (cRep) {
          if (cRep) return cRep;
          return fetch(e.request).then(fRep => {
            cache.put(e.request.url, fRep.clone());
            return fRep;
          });
          // return response || fetch(e.request);
        });

      })
    );
  }
});

// Update a service worker
self.addEventListener('activate', event => {
  var cacheWhitelist = [CACHE_NAME];
  event.waitUntil(
    caches.keys().then(cacheNames => {
      return Promise.all(
        cacheNames.map(cacheName => {
          if (cacheWhitelist.indexOf(cacheName) === -1) {
            return caches.delete(cacheName);
          }
        })
      );
    })
  );
});