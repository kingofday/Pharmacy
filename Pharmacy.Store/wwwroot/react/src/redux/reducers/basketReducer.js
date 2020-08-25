import actionTypes from '../actions/actionTypes';


const initState = {
    items: [],
    totalPrice: 0,
    totalDiscount: 0
};
const calculate = (items) => {
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
            var items = state.items;
            console.log(items);
            let idx = items.findIndex(x => x.drugId === action.payload.drugId);
            if (idx === -1)
                items.push(action.payload);
            else
                items[idx].count = action.payload.count;
            return { ...state, items: items, ...calculate(items) };
        case actionTypes.UPDATE_BASKET:
            let item = state.items.find(x => x.drugId == action.payload.drugId);
            if (item) item.count = action.payload.count;
            return { ...state, items: [...state.items], ...calculate(state.items) };
        case actionTypes.REMOVE_FROM_BASKET:
            state.items.splice(state.items.findIndex(x => x.drugId == action.payload.id), 1);
            return { ...state, items: [...state.items], ...calculate(state.items) };
        case actionTypes.CLEAR_BASKET:
            return { ...state, items: [], totalPrice: 0, totalDiscount: 0 };
        case actionTypes.SET_WHOLE:
            return { ...state, items: action.payload.items, ...calculate(action.payload.items) };
        case actionTypes.SET_BASKET_ROUTE:
            return { ...state, route: action.payload.route };
        case actionTypes.CLEAR_TEMP_BASKET:
            state.items = state.items.filter(x => !x.itemId);
            return { ...state, items: [...state.items], ...calculate(state.items) };
        case actionTypes.CHANGED_BASKET_ITEMS:
            action.payload.items.forEach(p => {
                let idx = state.items.findIndex(x => x.drugId === p.drugId);
                if (idx > -1) {
                    if (p.count === 0) state.items.splice(idx, 1);
                    else {
                        state.items[idx].price = p.price;
                        state.items[idx].discount = p.discount;
                        state.items[idx].realPrice = (p.price - p.discount);
                    }
                }

            });
            return { ...state, items: [...state.items], ...calculate(state.items) };
        default:
            return state;
    }
};