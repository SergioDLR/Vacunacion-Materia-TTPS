import { TableRow, TableCell, Button } from "@mui/material";
import CustomModal from "../../utils/Modal";
import ListaReglas from "./ListaReglas";
import { useState } from "react";
import CustomButton from "@/components/utils/CustomButtom";
const Vauna = ({ vacuna }) => {
  const [open, setOpen] = useState(false);
  return (
    <TableRow>
      <TableCell>{vacuna?.descripcionTipoVacuna}</TableCell>
      <TableCell align="right">{vacuna?.descripcion}</TableCell>
      <TableCell align="right">{vacuna?.descripcionPandemia}</TableCell>
      <TableCell align="right">{vacuna?.cantidadDosis}</TableCell>
      <TableCell align="right">
        <CustomModal title={"Ver"} open={open} setOpen={setOpen}>
          <ListaReglas dosis={vacuna?.dosis} />
          <CustomButton variant={"contained"} color={"error"} onClick={() => setOpen(false)}>
            Cerrar
          </CustomButton>
        </CustomModal>
      </TableCell>
    </TableRow>
  );
};

export default Vauna;
