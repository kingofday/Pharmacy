const baseUrl = 'https://localhost:44328/';

const addr = {
    getCategories: parentId => `${baseUrl}Category?parentId=${parentId}`,
    searchDrug: q => `${baseUrl}drug?q=${q}`,
    getSingleDrug: id => `${baseUrl}drug/${id}`,
}
export default addr;