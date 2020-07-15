import actionTypes from '../actions/actionTypes';


const initState = {
    items: [],
    totalPrice: 0,
    totalDiscount: 0,
    route: '/basket'
};
const caculate = (items) => {
    let totalPrice = items.reduce(function (total, x) {
        return total + (x.realPrice * x.count);
    }, 0);
    let totalDiscount = items.reduce(function (total, x) {
        return total + ((x.price - x.realPrice) * x.count);
    }, 0);
    return { totalPrice, totalDiscount };
}
export default function basketReducer(state = initState, action) {
    switch (action.type) {
        case actionTypes.ADD_TO_BASKET:
            state.items = state.items.filter(x => !x.itemId);
            let idx = state.items.findIndex(x => x.id == action.payload.id);
            let items = [];
            if (idx === -1)
                items = [...state.items, { ...action.payload }];
            else {
                state.items[idx].count = action.payload.count;
                items = [...state.items];
            }
            return { ...state, items, ...caculate(items) };
        case actionTypes.UPDATE_BASKET:
            let item = state.items.find(x => x.id == action.payload.id);
            if (item) item.count = action.payload.count;
            return { ...state, items: [...state.items], ...caculate(state.items) };
        case actionTypes.REMOVE_FROM_BASKET:
            state.items.splice(state.items.findIndex(x => x.id == action.payload.id), 1);
            return { ...state, items: [...state.items], ...caculate(state.items) };
        case actionTypes.CLEAR_BASKET:
            return { ...state, items: [], totalPrice: 0, totalDiscount: 0 };
        case actionTypes.SET_WHOLE:
            return { ...state, items: action.payload.items, ...caculate(action.payload.items) };
        case actionTypes.SET_BASKET_ROUTE:
            return { ...state, route: action.payload.route };
        case actionTypes.CLEAR_TEMP_BASKET:
            state.items = state.items.filter(x => !x.itemId);
            return { ...state, items: [...state.items], ...caculate(state.items) };
        case actionTypes.CHANGED_BASKET_ITEMS:
            action.payload.products.forEach(p => {
                let idx = state.items.findIndex(x => x.id === p.id);
                if (idx > -1) {
                    if (p.count === 0) state.items.splice(idx, 1);
                    else {
                        state.items[idx].price = p.price;
                        state.items[idx].discount = p.discount;
                        state.items[idx].realPrice = p.realPrice;
                    }
                }

            });
            return { ...state, items: [...state.items], ...caculate(state.items) };
        default:
            return state;
    }
};