import React from "react";
import { useState, useEffect } from "react";
import { useDispatch } from "react-redux";
import { useGetMenuItemsQuery } from "../../../Apis/menuItemApi";
import { menuItemModel } from "../../../Interfaces";
import { setMenuItem } from "../../../Storage/Redux/menuItemSlice";
import { MainLoader } from "../Common";
import MenuItemCard from "./MenuItemCard";

function MenuItemList() {
  //const [menuItems, setMenuItems] = useState<menuItemModel[]>([]);
  const dispatch = useDispatch();
  const { data, isLoading } = useGetMenuItemsQuery(null);

  useEffect(() => {
    if (!isLoading) {
      dispatch(setMenuItem(data.result));
    }
  }, [isLoading]);

  if (isLoading) {
    return <MainLoader />;
  }

  return (
    <div className="container row">
      {data.result.length > 0 &&
        data.result.map((menuItem: menuItemModel, index: number) => (
          <MenuItemCard menuItem={menuItem} key={index} />
        ))}{" "}
    </div>
  );
}

export default MenuItemList;
