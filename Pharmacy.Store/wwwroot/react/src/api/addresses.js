export const imagePrefixUrl = '';
//const baseUrl = 'https://pharma.hillavas.com/Api/';
const baseUrl = 'https://localhost:44328/';
//const baseUrl = 'https://test1.kingofday.ir/';
const addr = {
    getCategories: parentId => `${baseUrl}Category?parentId=${parentId}`,
    searchDrug: (filter) => `${baseUrl}drug?name=${filter.name ? filter.name : ''}&type=${filter.type || 0}&minPrice=${filter.minPrice || ''}&maxPrice=${filter.maxPrice || ''}&pageNumber=${filter.pageSize || 9}&pageNumber=${filter.pageNumber || 1}&pageSize=${filter.pageSize || 10}`,
    getSingleDrug: id => `${baseUrl}drug/${id}`,
    getDrugStores: `${baseUrl}DrugStore`,
    signUp: `${baseUrl}SignUp`,
    signIn: `${baseUrl}SignIn`,
    confirm: `${baseUrl}Confirm`,
    resendSMS: (mobileNumber) => `${baseUrl}Auth/${mobileNumber}`,
    getAddresses: `${baseUrl}Address`,
    addAddress: `${baseUrl}Address`,
    getDeliveries: `${baseUrl}DeliveryProvider`
}
export default addr;