/* eslint-disable no-console */
import {
  FormComponentProps,
  usePageCommand
} from "@kentico/xperience-admin-base";
import { ProgressBar } from "@kentico/xperience-admin-components";
import React, { useEffect, useState } from "react";

interface ExportResult {
  file: string;
}

type ExportStatus = "INACTIVE" | "EXPORTING" | "COMPLETE" | "FAILURE";
type ComponentState = {
  status: ExportStatus;
  percentComplete: number;
};

interface DataExportComponentProps extends FormComponentProps {
  fileNamePrefix: string;
}

export const DataExportComponent = (props: DataExportComponentProps) => {
  const [state, setState] = useState<ComponentState>({
    status: "INACTIVE",
    percentComplete: 0,
  });

  useEffect(() => {
    setState((s:any) => ({ ...s, status: "EXPORTING" }));
    dataExport()
      .then(() => setState((s:any) => ({ ...s, status: "COMPLETE" })))
      .catch((err:any) => {
        console.error(err);
        setState((s:any) => ({ ...s, status: "FAILURE" }));
      });

    let current = 0;
    const target = 100;
    const interval = 500; // 500 ms (half-second intervals)

    const intervalId = setInterval(() => {
      // Calculate the next increment as a percentage of the remaining distance to target
      const increment = (target - current) * 0.1;
      current += increment;

      // Ensure we don�t go beyond the target due to floating-point inaccuracies
      if (current >= target - 0.01) {
        // Tolerance for floating-point
        current = target;
        clearInterval(intervalId);
      }

      setState((s:any) => ({ ...s, percentComplete: current.toFixed(2) }));
    }, interval);

    return () => clearInterval(intervalId);
  }, []);

  const { execute: dataExport } = usePageCommand<ExportResult>(
    "EXPORT_LIST",
    {
      after: (resp:any) => {
        if (!resp) {
          return;
        }

        const binaryString = atob(resp.fileData);
        const binaryLength = binaryString.length;
        const bytes = new Uint8Array(binaryLength);
        for (let i = 0; i < binaryLength; i++) {
          bytes[i] = binaryString.charCodeAt(i);
        }

        // Create a Blob from the binary data
        const blob = new Blob([bytes], { type: "application/octet-stream" }); // Change MIME type if necessary

        // Create a download link
        const downloadUrl = URL.createObjectURL(blob);
        const link = document.createElement("a");
        link.href = downloadUrl;
        link.download = `${
          props.fileNamePrefix
        }_${getDashCaseDateSuffix()}.csv`; // Set the desired file name and extension here
        link.click();

        // Clean up the URL object
        URL.revokeObjectURL(downloadUrl);
      },
    }
  );

  return (
    <div>
    </div>
  );
};

function getDashCaseDateSuffix() {
  const date = new Date();
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, "0"); // Months are zero-based
  const day = String(date.getDate()).padStart(2, "0");

  return `${year}-${month}-${day}`;
}
