// Display uploaded photo
function displaySelectedPhoto() {
    const input = document.getElementById('photoInput');

    // Check if a file is selected
    if (input.files && input.files.length > 0) {
        // Get the selected file
        const file = input.files[0];

        // Create a FileReader to read the file as a data URL
        const reader = new FileReader();
        reader.onload = function (e) {
            // Display the selected photo
            const selectedPhoto = document.getElementById('selectedPhoto');
            const selectedPhotoContainer = document.getElementById('selectedPhotoContainer');
            selectedPhoto.src = e.target.result;
            selectedPhotoContainer.style.display = 'flex';
        };
        reader.readAsDataURL(file);
    }
}

// Function to remove the selected photo
function removeSelectedPhoto() {
    // Clear the file input
    const input = document.getElementById('photoInput');
    input.value = '';

    // Hide the selected photo container
    const selectedPhotoContainer = document.getElementById('selectedPhotoContainer');
    selectedPhotoContainer.style.display = 'none';
}