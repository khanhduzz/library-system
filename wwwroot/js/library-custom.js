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

document.addEventListener('DOMContentLoaded', function () {
    const navbar = document.querySelector('.navbar');

    window.addEventListener('scroll', () => {
        if (window.scrollY > 50) {
            navbar.classList.add('shrink');
        } else {
            navbar.classList.remove('shrink');
        }
    });

    const currentPath = window.location.pathname.toLowerCase();

    document.querySelectorAll('.navbar-nav .nav-link, .dropdown-item').forEach(link => {
        const href = link.getAttribute('href');
        if (!href) return;

        const normalizedHref = href.toLowerCase();

        // Special case for Home: match only exact "/"
        if (normalizedHref === "/" && currentPath === "/") {
            link.classList.add('active');
            return;
        }

        // Match others if currentPath starts with href + slash or is exact match
        if (normalizedHref !== "/" &&
            (currentPath === normalizedHref || currentPath.startsWith(normalizedHref + "/"))) {
            link.classList.add('active');

            const parentDropdown = link.closest('.dropdown');
            if (parentDropdown) {
                const toggle = parentDropdown.querySelector('.nav-link.dropdown-toggle');
                if (toggle) toggle.classList.add('active');
            }
        }
    });
});
