/* ------ Google Maps script ------ */
function initMap() {
    var myLatlng = { lat: -34.736, lng: 138.600 };
    var map = new google.maps.Map(document.getElementById('map'), { zoom: 17, center: myLatlng });
    var marker = new google.maps.Marker({ position: myLatlng, map: map });
}