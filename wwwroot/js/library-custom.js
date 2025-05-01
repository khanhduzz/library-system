function previewImage(event) {
	const file = event.target.files[0];
	const preview = document.getElementById('ImagePreview');
	const noImageText = document.getElementById('NoImageText');

	if (file) {
		const reader = new FileReader();
		reader.onload = function (e) {
			preview.src = e.target.result;
			preview.style.display = 'block';
			noImageText.style.display = 'none';
		};
		reader.readAsDataURL(file);
	} else {
		preview.style.display = 'none';
		noImageText.style.display = 'block';
	}
}