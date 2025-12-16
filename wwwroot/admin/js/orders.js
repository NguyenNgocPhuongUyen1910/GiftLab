// Orders JavaScript
(function() {
    'use strict';

    // Initialize when DOM is ready
    document.addEventListener('DOMContentLoaded', function() {
        initOrderFilters();
        initViewOrderModal();
        initChangeStatusModal();
        initFilterModal();
    });

    // Order Filter Buttons
    function initOrderFilters() {
        const filterButtons = document.querySelectorAll('.btn-group .btn');
        
        filterButtons.forEach(function(button) {
            button.addEventListener('click', function() {
                // Remove active class from all buttons
                filterButtons.forEach(function(btn) {
                    btn.classList.remove('active');
                });
                
                // Add active class to clicked button
                button.classList.add('active');
                
                const filter = button.textContent.trim();
                // TODO: Implement filter logic
                console.log('Filter orders by:', filter);
            });
        });
    }

    // View Order Modal - Load data when opened
    function initViewOrderModal() {
        const viewOrderModal = document.getElementById('viewOrderModal');
        if (viewOrderModal) {
            viewOrderModal.addEventListener('show.bs.modal', function (event) {
                const button = event.relatedTarget;
                const orderId = button.getAttribute('data-order-id');
                const customer = button.getAttribute('data-customer');
                const amount = button.getAttribute('data-amount');
                const status = button.getAttribute('data-status');
                const date = button.getAttribute('data-date');
                const items = button.getAttribute('data-items');
                
                document.getElementById('viewOrderId').textContent = orderId;
                document.getElementById('viewOrderId2').textContent = '#' + orderId;
                document.getElementById('viewOrderCustomer').textContent = customer;
                document.getElementById('viewOrderDate').textContent = date;
                document.getElementById('viewOrderTotal').textContent = '₫' + parseInt(amount).toLocaleString('vi-VN');
                
                // Set status badge
                const statusElement = document.getElementById('viewOrderStatus');
                const statusClass = status === 'Completed' || status === 'Hoàn thành' ? 'bg-success' : 
                                   status === 'Pending' || status === 'Đang chờ' ? 'bg-warning' : 
                                   status === 'Shipped' || status === 'Đã giao hàng' ? 'bg-info' : 'bg-secondary';
                const statusText = status === 'Completed' ? 'Hoàn thành' : 
                                  status === 'Pending' ? 'Đang chờ' : 
                                  status === 'Shipped' ? 'Đã giao hàng' : 
                                  status === 'Delivered' ? 'Đã nhận hàng' : 
                                  status === 'Cancelled' ? 'Đã hủy' : status;
                statusElement.innerHTML = `<span class="badge ${statusClass}">${statusText}</span>`;
                
                // Set order items (demo data)
                const itemsHtml = `
                    <tr>
                        <td>Rocky Road Cupcake</td>
                        <td>2</td>
                        <td>₫35,000</td>
                        <td>₫70,000</td>
                    </tr>
                    <tr>
                        <td>Melten Lava Cookie</td>
                        <td>1</td>
                        <td>₫25,000</td>
                        <td>₫25,000</td>
                    </tr>
                `;
                document.getElementById('viewOrderItems').innerHTML = itemsHtml;
                document.getElementById('viewOrderSubtotal').textContent = '₫' + (parseInt(amount) - 30000).toLocaleString('vi-VN');
            });
        }
    }

    // Change Status Modal - Load data when opened
    function initChangeStatusModal() {
        const changeStatusModal = document.getElementById('changeStatusModal');
        if (changeStatusModal) {
            changeStatusModal.addEventListener('show.bs.modal', function (event) {
                const button = event.relatedTarget;
                const orderId = button.getAttribute('data-order-id');
                const currentStatus = button.getAttribute('data-current-status');
                
                document.getElementById('changeStatusOrderId').value = orderId;
                document.getElementById('currentStatusDisplay').value = currentStatus;
            });
        }
    }

    // Filter Modal
    function initFilterModal() {
        const clearFiltersBtn = document.getElementById('clearOrderFiltersBtn');
        const applyFiltersBtn = document.getElementById('applyOrderFiltersBtn');
        const changeStatusBtn = document.getElementById('changeStatusBtn');
        
        if (clearFiltersBtn) {
            clearFiltersBtn.addEventListener('click', clearOrderFilters);
        }
        
        if (applyFiltersBtn) {
            applyFiltersBtn.addEventListener('click', applyOrderFilters);
        }
        
        if (changeStatusBtn) {
            changeStatusBtn.addEventListener('click', handleChangeStatus);
        }
    }

    // Make handleChangeStatus available before initFilterModal uses it
    window.handleChangeStatus = function() {
        const orderId = document.getElementById('changeStatusOrderId').value;
        const newStatus = document.getElementById('newStatus').value;
        const note = document.getElementById('statusNote').value;
        
        if (!newStatus) {
            alert('Vui lòng chọn trạng thái mới!');
            return;
        }
        
        // TODO: Call API to update status
        console.log('Change status:', { orderId, newStatus, note });
        
        const modal = bootstrap.Modal.getInstance(document.getElementById('changeStatusModal'));
        modal.hide();
        alert('Đã cập nhật trạng thái đơn hàng thành công!');
    };


    window.applyOrderFilters = function() {
        const status = document.getElementById('filterOrderStatus').value;
        const dateFrom = document.getElementById('filterOrderDateFrom').value;
        const dateTo = document.getElementById('filterOrderDateTo').value;
        const amountFrom = document.getElementById('filterOrderAmountFrom').value;
        const amountTo = document.getElementById('filterOrderAmountTo').value;
        
        // TODO: Apply filters
        console.log('Apply filters:', { status, dateFrom, dateTo, amountFrom, amountTo });
        
        const modal = bootstrap.Modal.getInstance(document.getElementById('filterOrderModal'));
        modal.hide();
    };

    window.clearOrderFilters = function() {
        document.getElementById('filterOrderStatus').value = '';
        document.getElementById('filterOrderDateFrom').value = '';
        document.getElementById('filterOrderDateTo').value = '';
        document.getElementById('filterOrderAmountFrom').value = '';
        document.getElementById('filterOrderAmountTo').value = '';
    };
})();

