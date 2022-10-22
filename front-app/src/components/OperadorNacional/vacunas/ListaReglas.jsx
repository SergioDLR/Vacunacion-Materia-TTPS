import { List, ListItem, ListItemText, Divider } from "@mui/material";
import CollapseReglas from "./CollapseReglas";
const ListaReglas = ({ dosis }) => {
  return (
    <List component="nav" aria-label="mailbox folders">
      {dosis.map((element, index) => (
        <CollapseReglas element={element} key={index} />
      ))}
    </List>
  );
};

export default ListaReglas;
