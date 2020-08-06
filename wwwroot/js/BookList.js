var datatable;

$(document).ready(function () {

    loadDataTable();
});

function loadDataTable() {

    datatable = $('#DT_Load').DataTable({
        "ajax": {
            "url": "/Book/getall/",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "author", "width": "15%" },
            { "data": "isbn", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                        <a href="/Book/Upsert?id=${data}" class='btn btn-success text-white'   style = 'cursor:pointer;width:60px;' >
                        Edit
                    </a>
                        &nbsp;
                        <a  class='btn btn-danger text-white'   style = 'cursor:pointer;width:60px;' OnClick=Delete('/book/Delete?id='+${data}) >
                            Delete
                    </a>
                        </div>`;

                }, "width": "15%"

            }
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });
}

function Delete(url) {
    swal(
        {
            title: "Are you sure to delete?",
            text: "Once deleted, will not able to recover",
            icon: "warning",
            DangerMode: true,
            buttons: true
        }).then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    type: "DELETE",
                    url: url,
                    success: function (data) {
                        if (data.success) {
                            toastr.success(data.message)
                            DataTable.ajax.reload();

                        }
                        else {
                            toastr.error(data.message);
                        }
                    }

                });
            }
        });
}