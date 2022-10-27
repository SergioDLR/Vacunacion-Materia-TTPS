import { TableRow, TableCell } from "@mui/material";
import priceParcer from "@/components/utils/pricerParser";
import dateParser from "@/components/utils/dateParser";
import numberParser from "@/components/utils/numberParser";
const Compra = ({ compra }) => {
  return (
    <TableRow>
      <TableCell align="center">{compra?.descripcionVacunaDesarrollada}</TableCell>
      <TableCell align="center">{compra?.idLote}</TableCell>
      <TableCell align="center">{compra?.descripcionEstadoCompra}</TableCell>
      <TableCell align="center">{numberParser(compra?.cantidadVacunas)}</TableCell>
      <TableCell align="center">{dateParser(compra?.fechaCompra)}</TableCell>
      <TableCell align="center">{dateParser(compra?.fechaEntrega)}</TableCell>
      <TableCell align="center">{numberParser(compra?.distribuidas)}</TableCell>
      <TableCell align="center">{numberParser(compra?.vencidas)}</TableCell>
      <TableCell align="right">{priceParcer(compra?.precioTotal)}</TableCell>
    </TableRow>
  );
};

export default Compra;
