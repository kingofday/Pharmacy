import React from 'react';
import { Link } from 'react-router-dom';
import DiscountBadg from './../../../shared/discountBadg';

export default class Product extends React.Component {
    render() {
        let product = this.props.product;
        return (
            <div className='product' title={product.name}>
                <Link to={`/product/${product.id}`}>
                    <img src={product.imgUrl} alt={product.name} />
                    {product.discount > 0 ? (<DiscountBadg discount={product.discount}/>) : null}
                </Link>
            </div>
        );
    }
}
