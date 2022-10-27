import { TableRow, TableCell, Button } from "@mui/material";
import dateParser from "@/components/utils/dateParser";
import numberParser from "@/components/utils/numberParser";
const Vacunado = ({ vacunado }) => {
  return (
    <TableRow>
      <TableCell>{numberParser(vacunado?.dni)}</TableCell>
      <TableCell align="right">{vacunado?.nombre}</TableCell>
      <TableCell align="right">{vacunado?.apellido}</TableCell>
      <TableCell align="right">{dateParser(vacunado?.fechaVacunacion)}</TableCell>
      <TableCell align="right">{vacunado?.descripcionJurisdiccion}</TableCell>
      <TableCell align="right">{vacunado?.descripcionVacuna}</TableCell>
      <TableCell align="right">{vacunado?.descripcionMarcaComercial}</TableCell>
      <TableCell align="right">{vacunado?.descripcionDosis}</TableCell>
    </TableRow>
  );
};

export default Vacunado;
