$(document).ready(function () {

    //Delete Product
    var productIdToDelete;
    $('.delete-product').click(function () {
        productIdToDelete = $(this).data('product-id');
    });
    $('#confirmDeleteButton').click(function () {
        $.ajax({
            type: 'POST',
            url: '/Product/Delete',
            data: { id: productIdToDelete },
            success: function (data) {
                $('#deleteConfirmationModal').modal('hide');
                location.reload();
            },
            error: function (error) {
                console.error('Error deleting product:', error);
            }
        });
    });

    // Delete Category
    var categoryIdToDelete;
    $('.delete-category').click(function() {
        categoryIdToDelete = $(this).data('category-id');
    })
    
    $('#categoryDeleteButton').click(function() {
        $.ajax({
            type: 'POST',
            url: '/Category/Delete',
            data: { id: categoryIdToDelete },
            success: function (data){
                $('#deleteCategoryModal').modal('hide');
                location.reload();
            },
            error: function(error){
                console.error('Error deleting product:', error);
            }
        });
    });

    // Delete Deal
    var dealIdToDelete
    $(".delete-deal").click(function (){
        dealIdToDelete = $(this).data('deal-id')
    })
    $("#dealDeleteButton").click(function (){
        $.ajax({
            type: 'POST',
            url: '/Deal/Delete',
            data: { id: dealIdToDelete },
            success: function (data){
                $('#deleteDealConfirmationModal').modal('hide')
                location.reload()
            },
            error: function (error){
                console.error('Error deleting deal:', error)
            }
        })
    })

    // Delete Testimonial
    var testimonialIdToDelete
    $('.delete-testimonial').click(function () {
        testimonialIdToDelete = $(this).data('testimonial-id')
    })
    $('#testimonialDeleteButton').click(function () {
        $.ajax({
            type: 'POST',
            url: '/Testimonial/Delete',
            data: { id: testimonialIdToDelete },
            success: function () {
                $('#deleteTestimonialConfirmationModal').modal('hide')
                location.reload()
            },
            error: function (error) {
                console.error('Error deleting testimonial:', error)
            }
        })
    })

});
