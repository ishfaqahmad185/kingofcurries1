


function CartAddIndexPage(productId, isSubItem, isFreeItem, subItemId, freeItemId) {

	var quantity = 1;
	var formData = new FormData();
	formData.append("id", productId);
	formData.append("quantity", quantity);
	formData.append("isSubItem", isSubItem);
	formData.append("isFreeItem", isFreeItem);
	formData.append("subItemId", subItemId);
	formData.append("freeItemId", freeItemId);

	$.ajax({
		type: "POST",
		url: "/Home/AddToCartItem/",

		data: formData,
		processData: false,
		contentType: false,
		dataType: "json",
		success: function (result) {
			if (result.success) {



				var Price = "";
				var Discount = "";
				var data = result.responseText
				$("#SubItemModel").modal('hide');
				toastShow(data, "Added to cart successfully");

				CartListJson();
			}
			else {
				const toast = new bootstrap.Toast(liveToast);

				toast.hide();
				swal("Info!", result.responseText, "info");
			}


		},
		error: function (errormessage) {
			//alert(errormessage.responseText);
		}
	});


}

function CartRemoveItem(productId) {

	var quantity = 1;
	var formData = new FormData();
	formData.append("id", productId);


	$.ajax({
		type: "POST",
		url: "/Home/RemoveItemFromCart/",

		data: formData,
		processData: false,
		contentType: false,
		dataType: "json",
		success: function (result) {
			if (result.success) {

				var Price = "";
				var Discount = "";
				var data = result.responseText
				toastShow(data, "Removed from cart successfully");

				CartListJson();
			}
			else {
				const toast = new bootstrap.Toast(liveToast);

				toast.hide();
				swal("Error!", result.responseText, "error");
			}


		},
		error: function (errormessage) {
			//alert(errormessage.responseText);
		}
	});


}

function CartUpdateItem(productSlug,quantity) {

	
	var formData = new FormData();
	formData.append("productSlug", productSlug);
	formData.append("quantity", quantity);


	$.ajax({
		type: "POST",
		url: "/Home/UpdateItemFromCart/",

		data: formData,
		processData: false,
		contentType: false,
		dataType: "json",
		success: function (result) {
			if (result.success) {

				
				CartListJson();
			}
			else {
				const toast = new bootstrap.Toast(liveToast);

				toast.hide();
				swal("Error!", result.responseText, "error");
			}


		},
		error: function (errormessage) {
			//alert(errormessage.responseText);
		}
	});


}


function generateBackgroundColor() {
	var letters = '0123456789ABCDEF'.split('');
	var color = '#';
	for (var i = 0; i < 6; i++) {
		color += letters[Math.round(Math.random() * 15)];
	}

	$('.person-circle').css("background-color", color);
}

function CartListJson() {




	$.ajax({
		type: "Get",
		url: "/Home/ListCartDataJson/",


		processData: false,
		contentType: false,
		dataType: "json",
		success: function (result) {
			if (result.isSuccess) {

				$(".cart-count").html(result.responseText.length);
				$("#spnSubPrice").html(result.subPrice.toFixed(2));
				var html = '';

				$.each(result.responseText, function (key, item) {
					var slug = "'" + item.productSlug + "'";
					html += '<li class="woocommerce-mini-cart-item mini_cart_item">';
					html += '<a onclick="CartRemoveItem(' + item.items.itemId + ')" class="remove remove_from_cart_button">';
					html += '<i class="far fa-times"></i>';
					html += '</a>';
					html += '<a href="#" style="width:210px">';
					html += '<img src="' + item.items.thumbnailImage + '" alt="Cart Image">';


					if (item.subItemId > 0 && item.freeItemId > 0) {
						html += '' + item.items.itemTitle + '<br/>' + item.subItems.subItemTitle + '<br/>' + item.freeItems.freeItemTitle + ' </a> ';
						html += '<span class="quantity">' + item.quantity + ' × ';
						html += '<span class="woocommerce-Price-amount amount">';
						html += '<span class="woocommerce-Price-currencySymbol">';
						html += '€</span>' + item.subItems.subItemPrice + '</span></span>';

						html += '</li>';
					}
					else if (item.subItemId > 0 && item.freeItemId <= 0) {
						html += '' + item.items.itemTitle + '<br/>' + item.subItems.subItemTitle + ' </a> ';
						html += '<span class="quantity">' + item.quantity + ' × ';
						html += '<span class="woocommerce-Price-amount amount">';
						html += '<span class="woocommerce-Price-currencySymbol">';
						html += '€</span>' + item.subItems.subItemPrice + '</span></span>';

						html += '</li>';
					}

					else if (item.subItemId <= 0 && item.freeItemId > 0) {
						html += '' + item.items.itemTitle + '<br/>' + item.freeItems.freeItemTitle + ' </a> ';
						html += '<span class="quantity">' + item.quantity + ' × ';
						html += '<span class="woocommerce-Price-amount amount">';
						html += '<span class="woocommerce-Price-currencySymbol">';
						html += '€</span>' + item.items.price + '</span></span>';
						

						html += '</li>';
					}
					else {
						html += '' + item.items.itemTitle + ' </a> ';
						html += '<span class="quantity">' + item.quantity + ' × ';
						html += '<span class="woocommerce-Price-amount amount">';
						html += '<span class="woocommerce-Price-currencySymbol">';
						html += '€</span>' + item.items.price + '</span></span>';
					
						

						html += '</li>';

					}
				});

				$(".product_list_widget").html(html);



			}
			else {

				swal("Error!", result.responseText, "error");
			}


		},
		error: function (errormessage) {
			//alert(errormessage.responseText);
		}
	});


}




function toastShow(objData, headertext) {
	console.log(objData);
	$(".toaster-header").html(headertext);
	var html = '';

	html += '<div class="row" style="background-color:white !important">';
	html += ' <div class="col-4">';
	html += '<img src="' + objData.itemImage + '" />';
	html += ' </div>';
	html += '  <div class="col-8">';
	html += ' <div class="row">';
	html += ' <div class="col-sm-12">';
	html += '<h5 style="color:Black !important">' + objData.itemTitle + '</h5>  ';
	html += '</div>';
	html += '</div>';


	html += '</div>  </div>';
	$(".toast-body").html(html);

	const toast = new bootstrap.Toast(liveToast)

	toast.show()


}

function SubItemShowModel(itemId, isSubItem, isFreeItem) {

	var formData = new FormData();

	formData.append("itemId", itemId);
	formData.append("isSubItem", isSubItem);
	formData.append("isFreeItem", isFreeItem);

	$.ajax({
		type: "POST",
		url: "/Menu/GetSubItemsData/",

		data: formData,
		processData: false,
		contentType: false,
		dataType: "json",
		success: function (result) {
			if (result.success) {
				console.log(result.jsonText);
				$("#btnAddToCart").attr("disabled");
				$("#btnAddToCart").addClass("disabled");
				var data = result.jsonText;
				$(".modal-title #itemTitle").html(data.item.itemTitle);		
				$(".modal-title #itemAllergen").html(" " + data.item.itemAllergen);
				$(".modal-body #itemDescription").html(data.item.itemDetail);
				$(".modal-body #itemPrice").html(data.item.price.toFixed(2));
				$(".modal-body #itemSubTitle").attr("data-id","false");
				$(".modal-body #itemSubTitle").html("");
				$(".modal-body #itemFreeTitle").html("");
				$("#hdnItemId").val(data.productId);
				$("#hdnIsSubItems").val(data.isSubItems);
				$("#hdnIsFreeItems").val(data.isFreeItems);
				$("#multiCollapseExample1").show();
				$("#multiCollapseExample2").hide();
				if (data.isSubItems) {
					BindListSubItems(data.listSubItems);
					$(".li-sub-items").attr("onclick", "SubMenuClick(this)");

					$("#divSubItems").removeAttr("hidden");
					$("#divSubItems").show();
					$("#SubItemModel").modal('show');
				} else {
					$(".ul-sub-items").html('');
					$("#divSubItems").attr("hidden");
					$("#divSubItems").hide();


				}

				if (data.isFreeItems) {

					if (data.isSubItems) {

					} else {
						$("#divFreeItems").removeAttr("hidden");
						$("#divFreeItems").show();
						$("#multiCollapseExample2").show();
					}

					BindListFreeItems(data.listFreeItems);
					$(".li-free-items").attr("onclick", "FreeMenuClick(this)");
				
				
				} else {
					$(".ul-free-items").html('');
					$("#divFreeItems").attr("hidden");
					$("#divFreeItems").hide();
				}

				if (data.isSubItems === false && data.isFreeItems === true) {
					
					$("#divFreeItems").removeAttr("hidden");
					$("#divFreeItems").show();
					$("#SubItemModel").modal('show');
				} 

			}
			else {

				swal("Error!", result.jsonText, "error");
			}


		},
		error: function (errormessage) {
			//alert(errormessage.responseText);
		}
	});



}

function SubMenuClick(e) {

	$(".li-sub-items").removeClass("active");

	
	$(e).addClass("active");
	$("#itemSubTitle").html($(".ul-sub-items .active").attr("data-title"));
	$("#itemPrice").html($(".ul-sub-items .active").attr("data-price"));
	
	if ($("#hdnIsFreeItems").val() === "true") {
		$("#divFreeItems").removeAttr("hidden");
		
		$("#multiCollapseExample1").hide();
		$("#multiCollapseExample2").show();
		
		$("#itemSubTitle").html($(".ul-sub-items .active").attr("data-title"));
		$("#itemSubTitle").attr("data-id", "true");
		$("#itemPrice").html($(".ul-sub-items .active").attr("data-price"));
		$(".subItemArrow").removeClass("fa-chevron-up");
		$(".subItemArrow").addClass("fa-chevron-down");
		$("#divFreeItems").show();


	} else {
	
		$("#btnAddToCart").removeAttr("disabled");
		$("#btnAddToCart").removeClass("disabled");
	}
	
	





}

function FreeMenuClick(e) {

	$(".li-free-items").removeClass("active");
	$("#btnAddToCart").removeAttr("disabled");
	$("#btnAddToCart").removeClass("disabled");

	$(e).addClass("active");

	$("#itemFreeTitle").html("(" +$(".ul-free-items .active").attr("data-title") +")")






}

function BindListSubItems(listData) {

	var htmlData = "";

	$.each(listData, function (key, item) {

		htmlData += '<li class="list-group-item  li-sub-items" data-id="' + item.subItemId + '" data-title="' + item.subItemTitle + '" data-price="' + item.subItemPrice.toFixed(2) +'">';

		htmlData += '<img src="' + item.subItemThumbnail + '" width="30px" style="border-radius:20%" />';

		htmlData += '<span class="pl-5">' + item.subItemTitle + '</span>';
		htmlData += '<span class=" text-right  pvertical-center "><b>€' + item.subItemPrice.toFixed(2) + '</b></span>';
		htmlData += '</li>';


	});

	$(".ul-sub-items").html(htmlData);
	$("#divSubItems").removeAttr("hidden");
}

function BindListFreeItems(listData) {
	console.log("isFree Items");
	console.log(listData);
	var htmlData = "";

	$.each(listData, function (key, item) {

		htmlData += '<li class="list-group-item  li-free-items" data-id="' + item.freeItemId + '"  data-title="' + item.freeItemTitle + '">';

		htmlData += '<img src="' + item.freeItemThumbnail + '" width="30px" style="border-radius:20%" />';

		htmlData += '<span class="pl-5">' + item.freeItemTitle + '</span>';

		htmlData += '</li>';


	});
	console.log(htmlData);
	$(".ul-free-items").html(htmlData);
	

}

$('#btnAddToCart').click(function () {
	var itemId = $("#hdnItemId").val();
	var isSubItem = $("#hdnIsSubItems").val();
	var isFreeItem = $("#hdnIsFreeItems").val();
	var subItemId = $(".ul-sub-items .active").attr("data-id");
	var freeItemId = $(".ul-free-items .active").attr("data-id");




	

	if (itemId > 0) {

		if (isSubItem.toLowerCase()==="true") {
			
			if (subItemId == undefined) {
				swal("Error!", "Kindly Select At Least 1 Sub Items");
				return;
			}
		}

		if (isFreeItem.toLowerCase()==="true") {
			
			if (freeItemId === undefined) {
				
				swal("Error!", "Kindly Select At Least 1 Free Items");
				return;
			}
		}

		CartAddIndexPage(itemId, isSubItem, isFreeItem, subItemId, freeItemId);

	} else {
		swal("Error!", "Some Error Occured", "error");
		return;
	}


});


function CheckOutListJsonData() {



	$.ajax({
		type: "Get",
		url: "/Home/ListCartDataJson/",


		processData: false,
		contentType: false,
		dataType: "json",
		success: function (result) {
			if (result.isSuccess) {

				console.log(result);
				var html = '';
			
				$(".tdSubTotal").html(result.subPrice.toFixed(2));
				if (result.subPrice.toFixed(2) >= 30) {
					$(".tdDeliveryCharges").html("0 Free Delivery");
					$(".tdGrandTotal").html( (parseFloat(result.subPrice)).toFixed(2));

				} else {
					$(".tdDeliveryCharges").html(result.deliveryCharges);
					$(".tdGrandTotal").html((parseFloat(result.deliveryCharges) + parseFloat(result.subPrice)).toFixed(2));

				}
				
				$(".tdGrandTotal").html((parseFloat(result.deliveryCharges) + parseFloat(result.subPrice)).toFixed(2));
				var totalSum = parseFloat($(".carttableTotal").text());
				$(".carttableTotal").html(totalSum.toFixed(2));
				$.each(result.responseText, function (key, item) {
					var slug = "'" + item.productSlug + "'";
					html += '<li class="woocommerce-mini-cart-item mini_cart_item">';
				
					
					html += '<img src="' + item.items.thumbnailImage + '" alt="Cart Image">';


					if (item.subItemId > 0 && item.freeItemId > 0) {
						html += '' + item.items.itemTitle + '<br/>' + item.subItems.subItemTitle + '<br/>' + item.freeItems.freeItemTitle + '  ';
						html += '<span class="quantity">' + item.quantity + ' × ';
						html += '<span class="woocommerce-Price-amount amount">';
						html += '<span class="woocommerce-Price-currencySymbol">';
						html += '€</span>' + item.subItems.subItemPrice + '</span></span>';

						html += '</li>';
					}
					else if (item.subItemId > 0 && item.freeItemId <= 0) {
						html += '' + item.items.itemTitle + '<br/>' + item.subItems.subItemTitle + ' </a> ';
						html += '<span class="quantity">' + item.quantity + ' × ';
						html += '<span class="woocommerce-Price-amount amount">';
						html += '<span class="woocommerce-Price-currencySymbol">';
						html += '€</span>' + item.subItems.subItemPrice + '</span></span>';

						html += '</li>';
					}

					else if (item.subItemId <= 0 && item.freeItemId > 0) {
						html += '' + item.items.itemTitle + '<br/>' + item.freeItems.freeItemTitle + ' </a> ';
						html += '<span class="quantity">' + item.quantity + ' × ';
						html += '<span class="woocommerce-Price-amount amount">';
						html += '<span class="woocommerce-Price-currencySymbol">';
						html += '€</span>' + item.items.price + '</span></span>';


						html += '</li>';
					}
					else {
						html += '' + item.items.itemTitle + ' </a> ';
						html += '<span class="quantity">' + item.quantity + ' × ';
						html += '<span class="woocommerce-Price-amount amount">';
						html += '<span class="woocommerce-Price-currencySymbol">';
						html += '€</span>' + item.items.price + '</span></span>';



						html += '</li>';

					}
				});

				$(".checkOutTablebody").html(html);
			



			}
			else {

				swal("Error!", result.responseText, "error");
			}


		},
		error: function (errormessage) {
			//alert(errormessage.responseText);
		}
	});




}

$("#btnCollapseDiv").click(function () {
	if ($("#itemSubTitle").attr("data-id") === "true") {
		if ($(".subItemArrow").hasClass("fa-chevron-up")) {
			$(".subItemArrow").removeClass("fa-chevron-up");
			$(".subItemArrow").addClass("fa-chevron-down");
			$("#multiCollapseExample1").hide();
			$("#multiCollapseExample2").show();
			
		} else {
			$(".subItemArrow").removeClass("fa-chevron-down");
			$(".subItemArrow").addClass("fa-chevron-up");
			$("#multiCollapseExample1").show();
			$("#multiCollapseExample2").hide();
		}
	}
});
