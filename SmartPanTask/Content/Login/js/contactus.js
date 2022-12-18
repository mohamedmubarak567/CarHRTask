/* google map function custom */
function init_map() {
	var myOptions = {
		zoom: 10,
		center: new google.maps.LatLng(51.5073509, -0.12775829999998223),
		mapTypeId: google.maps.MapTypeId.ROADMAP,
		// This is where you would paste any style found on Snazzy Maps.
		styles: [ 
		{"featureType":"all","elementType":"labels.text.fill","stylers":[{"saturation":36},{"color":"#000000"},{"lightness":40}]},
		{"featureType":"all","elementType":"labels.text.stroke","stylers":[{"visibility":"on"},{"color":"#000000"},{"lightness":16}]},
		{"featureType":"all","elementType":"labels.icon","stylers":[{"visibility":"off"}]},
		{"featureType":"administrative","elementType":"geometry.fill","stylers":[{"color":"#000000"},{"lightness":20}]},
		{"featureType":"administrative","elementType":"geometry.stroke","stylers":[{"color":"#000000"},{"lightness":17},{"weight":1.2}]},
		{"featureType":"landscape","elementType":"geometry","stylers":[{"color":"#000000"},{"lightness":20}]},
		{"featureType":"poi","elementType":"geometry","stylers":[{"color":"#000000"},{"lightness":21}]},
		{"featureType":"road.highway","elementType":"geometry.fill","stylers":[{"color":"#000000"},{"lightness":17}]},
		{"featureType":"road.highway","elementType":"geometry.stroke","stylers":[{"color":"#000000"},{"lightness":29},{"weight":0.2}]},
		{"featureType":"road.arterial","elementType":"geometry","stylers":[{"color":"#000000"},{"lightness":18}]},
		{"featureType":"road.local","elementType":"geometry","stylers":[{"color":"#000000"},{"lightness":16}]},
		{"featureType":"transit","elementType":"geometry","stylers":[{"color":"#000000"},{"lightness":19}]},
		{"featureType":"water","elementType":"geometry","stylers":[{"color":"#000000"},{"lightness":17}]}
		]
	};
	/* Let's also add a marker while we're at it */
	map = new google.maps.Map(document.getElementById('gmap_canvas'), myOptions);
	marker = new google.maps.Marker({
		map: map,
		position: new google.maps.LatLng(51.5073509, -0.12775829999998223)
	});
	
	/* marker on click show infowindow */
	infowindow = new google.maps.InfoWindow({
		content: '<strong>Title</strong><br>London, United Kingdom<br>'
	});
	google.maps.event.addListener(marker, 'click', function() {
		infowindow.open(map, marker);
	});
}
google.maps.event.addDomListener(window, 'load', init_map);