
const baseUrl = window.location.origin.indexOf('localhost:3000') >= 0 ? 'https://localhost:44328/' : 'https://pharma.hillavas.com/api/';

const addr = {
    getCategories: (parentId) => `${baseUrl}Category?parentId=${parentId}`,
    searchDrug: (filter) => `${baseUrl}drug?name=${filter.name ? filter.name : ''}&type=${filter.type || 0}&categoryId=${filter.categoryId || ''}&minPrice=${filter.minPrice || ''}&maxPrice=${filter.maxPrice || ''}&pageSize=${filter.pageSize || 9}&pageNumber=${filter.pageNumber || 1}`,
    getSingleDrug: id => `${baseUrl}drug/${id}`,
    getDrugStores: `${baseUrl}DrugStore`,
    signUp: `${baseUrl}SignUp`,
    signIn: `${baseUrl}SignIn`,
    confirm: `${baseUrl}Confirm`,
    resendSMS: (mobileNumber) => `${baseUrl}Auth/${mobileNumber}`,
    getAddresses: `${baseUrl}Address`,
    addAddress: `${baseUrl}Address`,
    updateAddress: `${baseUrl}Address`,
    getDeliveries: `${baseUrl}DeliveryProvider`,
    getDeliveryPrice: (id) => `${baseUrl}DeliveryProvider/${id}`,
    payDeliveryPrice: (id) => `${baseUrl}DeliveryProvider/${id}`,
    addOrder: `${baseUrl}Order`,
    addPrescription: `${baseUrl}Prescription`,
    getPrescription: (id) => `${baseUrl}Prescription?id=${id}`,
}
export default addr;