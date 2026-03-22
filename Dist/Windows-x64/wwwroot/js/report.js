window.reportFunctions = {
    printReport: function() {
        window.print();
    },

    downloadData: function(filename, contentType, content) {
        const file = new Blob([content], {type: contentType});
        const a = document.createElement("a");
        const url = URL.createObjectURL(file);
        a.href = url;
        a.download = filename;
        document.body.appendChild(a);
        a.click();
        setTimeout(function() {
            document.body.removeChild(a);
            window.URL.revokeObjectURL(url);
        }, 0);
    },

    downloadPdf: function(filename, markdownContent) {
        if (typeof marked === 'undefined' || typeof html2pdf === 'undefined') {
            console.error("Required libraries (marked or html2pdf) not loaded.");
            alert("Could not load PDF generation tools. Please refresh and try again.");
            return;
        }

        // Convert Markdown to HTML
        const htmlContent = marked.parse(markdownContent);

        // Create a temporary container
        const container = document.createElement("div");
        container.innerHTML = `
            <div style="font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; color: #1a1a1a; padding: 40px; line-height: 1.6;">
                ${htmlContent}
            </div>
        `;

        // Configure PDF options
        const opt = {
            margin:       10,
            filename:     filename,
            image:        { type: 'jpeg', quality: 0.98 },
            html2canvas:  { scale: 2 },
            jsPDF:        { unit: 'mm', format: 'a4', orientation: 'portrait' }
        };

        // Generate and download
        html2pdf().set(opt).from(container).save();
    }
};
