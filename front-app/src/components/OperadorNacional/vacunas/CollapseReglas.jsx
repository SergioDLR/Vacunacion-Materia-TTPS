import { List, ListItem, ListItemText, Divider, ListItemIcon, ListItemButton, Collapse } from "@mui/material";
import ExpandLess from "@mui/icons-material/ExpandLess";
import ExpandMore from "@mui/icons-material/ExpandMore";
import { useState } from "react";
const CollapseReglas = ({ element }) => {
  const [open, setOpen] = useState(false);
  const handleClick = () => {
    setOpen(!open);
  };
  return (
    <>
      <ListItemButton onClick={handleClick}>
        <ListItemText primary={element.descripcion} />
        {open ? <ExpandLess /> : <ExpandMore />}
      </ListItemButton>
      <Collapse in={open} timeout="auto" unmountOnExit>
        <List component="div" disablePadding>
          {element.reglas.map((regla, index) => (
            <div key={index}>
              <Divider />
              <ListItemText sx={{ pl: 4 }}>Descripcion: {regla.descripcion}</ListItemText>
              <Divider />
              <ListItemText sx={{ pl: 4 }}>Lapso minimo dias: {regla.lapsoMaximoDias}</ListItemText>
              <Divider />
              <ListItemText sx={{ pl: 4 }}>Lapso maximo dias: {regla.lapsoMinimoDias}</ListItemText>
            </div>
          ))}
        </List>
      </Collapse>
      <Divider />
    </>
  );
};

export default CollapseReglas;
