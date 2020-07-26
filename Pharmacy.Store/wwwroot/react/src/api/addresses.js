const baseUrl = 'https://localhost:44328/';

const addr = {
    getCategories: parentId => `${baseUrl}Category?parentId=${parentId}`,
    searchDrug: (filter) => `${baseUrl}drug?name=${filter.name?filter.name:null}&type=${filter.type||0}&minPrice=${filter.minPrice||0}&maxPrie=${filter.maxPrice||0}&pageNumber=${filter.pageNumber||1}&pageSize=${filter.pageSize||10}`,
    getSingleDrug: id => `${baseUrl}drug/${id}`,
}
export default addr;