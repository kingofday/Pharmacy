export const imagePrefixUrl ='';
const baseUrl = 'https://localhost:44328/';
const addr = {
    getCategories: parentId => `${baseUrl}Category?parentId=${parentId}`,
    searchDrug: (filter) => `${baseUrl}drug?name=${filter.name?filter.name:''}&type=${filter.type||0}&minPrice=${filter.minPrice||''}&maxPrice=${filter.maxPrice||''}&pageNumber=${filter.pageNumber||1}&pageSize=${filter.pageSize||10}`,
    getSingleDrug: id => `${baseUrl}drug/${id}`,
    getDrugStores:`${baseUrl}DrugStore`
}
export default addr;