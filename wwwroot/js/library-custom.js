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

$('#deleteModal').on('show.bs.modal', function (event) {
	var button = $(event.relatedTarget);
	var bookId = button.data('id');
	var bookTitle = button.data('title');

	var modal = $(this);
	modal.find('#modalBookTitle').text(bookTitle);
	modal.find('#deleteForm').attr('action', '/Books/Delete/' + bookId);
});